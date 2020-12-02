namespace goOfflineE.Services
{
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ContentService" />.
    /// </summary>
    public class ContentService : IContentService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        public ContentService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// The ContentDistribution.
        /// </summary>
        /// <param name="model">The model<see cref="Distribution"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ContentDistribution(Distribution model)
        {
            // Create new content
            var contentDistributionId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

            var newContent = new Entites.Distribution(model.SchoolId, contentDistributionId)
            {
                ClassId = model.ClassId,
                ContentId = model.ContentId,
                Active = true,
                CreatedBy = model.CreatedBy,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.CreatedBy,
            };

            try
            {
                await _tableStorage.AddAsync("Distribution", newContent);
            }
            catch (Exception ex)
            {

                throw new AppException("Content distribution error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="Content"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateUpdate(Content model)
        {
            var contents = await _tableStorage.GetAllAsync<Entites.Content>("Content");
            var content = contents.SingleOrDefault(user => user.RowKey == model.Id);

            if (content != null)
            {
                //Update class information
                content.CourseName = model.CourseName;
                content.CourseDescription = model.CourseDescription;
                content.CourseLevel = model.CourseLevel;
                content.CourseAssessment = model.CourseAssessment;
                content.Active = model.Active;
                content.UpdatedOn = DateTime.UtcNow;
                content.UpdatedBy = model.CreatedBy;

                try
                {
                    await _tableStorage.UpdateAsync("Content", content);
                }
                catch (Exception ex)
                {
                    throw new AppException("update content error: ", ex.InnerException);
                }

            }
            else
            {
                // Create new content
                var contentId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;

                var newContent = new Entites.Content(model.CourseCategory, contentId)
                {
                    CourseName = model.CourseName,
                    CourseDescription = model.CourseDescription,
                    CourseLevel = model.CourseLevel,
                    CourseAssessment = model.CourseAssessment,
                    CourseURL = model.CourseURL,
                    ThumbnailURL = model.ThumbnailURL,
                    Active = true,
                    CreatedBy = model.CreatedBy,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = model.CreatedBy,
                };
                try
                {
                    await _tableStorage.AddAsync("Content", newContent);
                }
                catch (Exception ex)
                {

                    throw new AppException("Create content error: ", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Content}}"/>.</returns>
        public async Task<IEnumerable<Content>> GetAll()
        {
            var contents = await _tableStorage.GetAllAsync<Entites.Content>("Content");
            contents = from content in contents
                       where content.Active.GetValueOrDefault(false)
                       select content;
            return MapContent(contents);
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Content}}"/>.</returns>
        public async Task<IEnumerable<Content>> GetAll(string schoolId, string classId)
        {
            var distributedContent = await _tableStorage.GetAllAsync<Entites.Distribution>("Distribution");

            var contents = await _tableStorage.GetAllAsync<Entites.Content>("Content");

            contents = from dcontent in distributedContent.Where(d => d.PartitionKey == schoolId && d.ClassId == classId)
                       join content in contents
                            on dcontent.ContentId equals content.RowKey
                       orderby content.UpdatedOn descending
                       where content.Active.GetValueOrDefault(false)
                       select content;

            return MapContent(contents);
        }

        /// <summary>
        /// The MapContent.
        /// </summary>
        /// <param name="contents">The contents<see cref="IEnumerable{Entites.Content}"/>.</param>
        /// <returns>The <see cref="IEnumerable{Content}"/>.</returns>
        private IEnumerable<Content> MapContent(IEnumerable<Entites.Content> contents)
        {
            var contentList = new List<Content>();

            foreach (var content in contents)
            {
                contentList.Add(new Content
                {
                    Id = content.RowKey,
                    CourseCategory = content.PartitionKey,
                    CourseLevel = content.CourseLevel,
                    CourseAssessment = content.CourseAssessment,
                    CourseName = content.CourseName,
                    CourseDescription = content.CourseDescription,
                    CourseURL = content.CourseURL,
                    ThumbnailURL = content.ThumbnailURL,
                    Active = content.Active,
                });
            }

            return contentList;
        }
    }
}
