namespace goOfflineE.Services
{
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AssignmentService" />.
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        public AssignmentService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// The CreateStudentAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateStudentAssigments(StudentAssignment model)
        {
            var assignments = await _tableStorage.GetAllAsync<Entites.Assignment>("Assignment");
            var assignment = assignments.SingleOrDefault(asmt => asmt.RowKey == model.Id);

            if (assignment != null)
            {
                assignment.UpdatedOn = DateTime.UtcNow;

                var studentAssignments = JsonConvert.SerializeObject(
                    new StudentAssignment { 
                        StudentName = model.StudentName, 
                        AssignmentURL = model.AssignmentURL, 
                        Id = model.Id,
                        CreatedDate = DateTime.UtcNow
                    }
                    );
                assignment.UpdatedBy = model.StudentName;
                assignment.StudentAssigments = studentAssignments;
               
                try
                {
                    await _tableStorage.UpdateAsync("Assignment", assignment);
                }
                catch (Exception ex)
                {
                    throw new AppException("Create student assigment error: ", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// The CreateTeacherAssigments.
        /// </summary>
        /// <param name="model">The model<see cref="TeacherAssignment"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateTeacherAssigments(TeacherAssignment model)
        {
            var assignmentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

            var newStudent = new Entites.Assignment(model.SchoolId, assignmentId)
            {

                AssignmentName = model.AssignmentName,
                AssignmentDescription = model.AssignmentDescription,
                AssignmentURL = model.AssignmentURL,
                ClassId = model.ClassId,
                SubjectName = model.SubjectName,

                Active = true,
                CreatedBy = model.CreatedBy,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.CreatedBy,
            };
            try
            {
                await _tableStorage.AddAsync("Assignment", newStudent);
            }
            catch (Exception ex)
            {
                throw new AppException("Create teacher assignments error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The GetAssignments.
        /// </summary>
        /// <param name="className">The className<see cref="string"/>.</param>
        /// <param name="subjectName">The subjectName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{TeacherAssignment}}"/>.</returns>
        public async Task<IEnumerable<TeacherAssignment>> GetAssignments(string schoolId, string classId)
        {
            TableQuery<Entites.Assignment> assignmentQuery = new TableQuery<Entites.Assignment>()
                 .Where(TableQuery.GenerateFilterCondition("ClassId", QueryComparisons.Equal, classId));
            var assignments = await _tableStorage.QueryAsync<Entites.Assignment>("Assignment", assignmentQuery);

            var assignmentList = from assignment in assignments
                                 where assignment.Active.GetValueOrDefault(false)
                                 orderby assignment.UpdatedOn descending
                                 select new TeacherAssignment
                                 {
                                     Id = assignment.RowKey,
                                     CreatedDate = assignment.Timestamp.DateTime,
                                     AssignmentName = assignment.AssignmentName,
                                     AssignmentDescription = assignment.AssignmentDescription,
                                     AssignmentURL = assignment.AssignmentURL,
                                     SubjectName = assignment.SubjectName,
                                     StudentAssignments = assignment.StudentAssigments
                                 };

            return assignmentList;
        }
    }
}
