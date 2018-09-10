// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MemoryWatcher.cs" company="Roche Diagnostics International Ltd">
//    Copyright (c) 2018 Roche Diagnostics International Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class MemoryWatcher
    {
        private readonly List<double> measuredMemoryConsumption = new List<double>();

        public bool Finished { get; private set; }

        public async void Start(TimeSpan measurementCycle)
        {
            var startingMemory = Process.GetCurrentProcess().WorkingSet64;

            while (!Finished)
            {
                //this.measuredMemoryConsumption.Add(Process.GetCurrentProcess().WorkingSet64 - startingMemory);

                await Task.Run(() =>
                 {
                     this.measuredMemoryConsumption.Add(Process.GetCurrentProcess().WorkingSet64 - startingMemory);
                 });
                await Task.Delay(measurementCycle);
            }
        }

        public void Stop()
        {
            this.Finished = true;
        }

        public IReadOnlyCollection<double> GetMeasuredTime()
        {
            if (!this.Finished)
            {
                throw new ArgumentException("Wait until Stop is called.");
            }

            return this.measuredMemoryConsumption.AsReadOnly();
        }
    }
}