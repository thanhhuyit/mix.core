﻿using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Quartz.Services
{
    // Ref: https://andrewlock.net/creating-a-quartz-net-hosted-service-with-asp-net-core/
    public class QuartzHostedService : IHostedService
    {
        private readonly IEnumerable<MixJobBase> _jobSchedules;
        private readonly QuartzService _service;

        public QuartzHostedService(
            IEnumerable<MixJobBase> jobSchedules, QuartzService scheduler)
        {
            _jobSchedules = jobSchedules;
            _service = scheduler;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {

            foreach (var jobSchedule in _jobSchedules)
            {
                if (jobSchedule.Schedule != null && !await _service.CheckExist(jobSchedule.Schedule.Name))
                {
                    jobSchedule.JobName = typeof(JobSchedule).FullName;
                    await _service.ScheduleJob(jobSchedule, cancellationToken);
                }
            }

            await _service.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _service.Shutdown(cancellationToken);
        }
    }
}