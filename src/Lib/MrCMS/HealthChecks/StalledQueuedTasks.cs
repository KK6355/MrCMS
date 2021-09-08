using System.Collections.Generic;
using System.Threading.Tasks;
using MrCMS.Helpers;
using MrCMS.Tasks;
using MrCMS.Website;
using NHibernate;

namespace MrCMS.HealthChecks
{
    public class StalledQueuedTasks : HealthCheck
    {
        private readonly IGetDateTimeNow _getDateTimeNow;
        private readonly ISession _session;

        public StalledQueuedTasks(ISession session, IGetDateTimeNow getDateTimeNow)
        {
            _session = session;
            _getDateTimeNow = getDateTimeNow;
        }

        public override string DisplayName => "Stalled Queued Tasks";

        public override async Task<HealthCheckResult> PerformCheck()
        {
            var checkDate = _getDateTimeNow.LocalNow.AddMinutes(-30);
            var any = await _session.QueryOver<QueuedTask>()
                .Where(
                    task => task.Status == TaskExecutionStatus.Pending &&
                            task.CreatedOn <= checkDate)
                .AnyAsync();
            return any
                ? new HealthCheckResult
                {
                    Messages = new List<string>
                    {
                        "One or more tasks have not been run in the last 30 minutes. " +
                        "Please check that your scheduler is still configured correctly."
                    }
                }
                : HealthCheckResult.Success;
        }
    }
}