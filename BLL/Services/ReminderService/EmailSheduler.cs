using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.MailingService.Interfaces;
using IDAL.Interfaces;
using Quartz;
using Quartz.Impl;

namespace BLL.Services.ReminderService
{
    public class EmailSheduler
    {
        public static void Start(IUnitOfWork unitOfWork, IMailingService mailingService)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            scheduler.Context.Put("unitOfWork", unitOfWork);
            scheduler.Context.Put("mailingService", mailingService);

            IJobDetail job = JobBuilder.Create<Reminder>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "remind_group")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
