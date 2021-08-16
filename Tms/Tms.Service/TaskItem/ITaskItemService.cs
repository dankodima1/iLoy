using System.Collections.Generic;
using System.Threading.Tasks;

using Tms.Data.Domain;

namespace Tms.Service
{
    public interface ITaskItemService
    {
        Task<TaskItem> GetAsync(int entityId);
        Task<List<TaskItem>> GetAsync();
        Task<List<TaskItem>> GetTasksOnlyAsync();

        Task<TaskItem> CreateAsync(TaskItem entity);
        Task UpdateAsync(TaskItem entity);
        //Task DeleteAsync(int entityId);
        Task DeleteAsync(TaskItem entity);
    }
}
