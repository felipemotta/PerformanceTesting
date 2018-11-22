// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MemoryWatcher.cs" company="Roche Diagnostics International Ltd">
//    Copyright (c) 2018 Roche Diagnostics International Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DevExperience.Assembly.Performance.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <inheritdoc />
    public class MemoryWatcher : IMemoryWatcher
    {
        private Task saveMemoryConsumptionTask;

        /// <summary>
        /// The measured memory consumption list.
        /// </summary>
        protected IList<long> MeasuredMemoryConsumption;

        /// <inheritdoc />
        public bool Finished { get; protected set; }

        /// <inheritdoc />
        public void Start(TimeSpan measurementCycle)
        {
            this.MeasuredMemoryConsumption = new List<long>();

            var startingMemory = Process.GetCurrentProcess().WorkingSet64;

            this.saveMemoryConsumptionTask = this.CreateSaveMemoryConsumptionTask(startingMemory, measurementCycle);

            this.saveMemoryConsumptionTask.Start();
        }

        private Task CreateSaveMemoryConsumptionTask(long startingMemory, TimeSpan measurementCycle)
        {
            return new Task(() =>
            {
                while (!this.Finished)
                {
                    var consumedMemory = Process.GetCurrentProcess().WorkingSet64 - startingMemory;
                    this.MeasuredMemoryConsumption.Add(consumedMemory);
                    Thread.Sleep(measurementCycle);
                }
            });
        }

        /// <inheritdoc />
        public IReadOnlyCollection<long> GetMeasuredMemory()
        {
            if (!this.Finished)
            {
                throw new ArgumentException("Wait until the watcher is stoped.");
            }

            if (!this.MeasuredMemoryConsumption.Any())
            {
                throw new ArgumentException("No memory consumption was logged.");
            }

            return new ReadOnlyCollection<long>(this.MeasuredMemoryConsumption);
        }

        /// <inheritdoc />
        public void Stop()
        {
            this.Finished = true;
            this.saveMemoryConsumptionTask?.Wait();
        }
    }

    /// <summary>
    /// The memory watcher interface.
    /// </summary>
    public interface IMemoryWatcher
    {
        /// <summary>
        /// It represents the status of the memory watcher process.
        /// True when the process is finished, otherwise false.
        /// </summary>
        bool Finished { get; }

        /// <summary>
        /// It starts the memory watcher for the current process.
        /// </summary>
        /// <param name="measurementCycle">the mesurement cycle to use during the memory watcher process.</param>
        void Start(TimeSpan measurementCycle);

        /// <summary>
        /// Gets the measured memory.
        /// </summary>
        /// <returns>A read only collection contaning the consumed memory per cycle.</returns>
        /// <exception cref="T:System.ArgumentException">Throws when the watcher is not finished or could not mesure memory consumption.</exception>
        IReadOnlyCollection<long> GetMeasuredMemory();

        /// <summary>
        /// It stops the memory watcher.
        /// </summary>
        void Stop();
    }

    namespace Roche.RMS.Calculations.Testing.Utilities.Performance.Extensions
    {
        using System.Linq;

        /// <summary>
        /// The memory watcher extensions class.
        /// </summary>
        public static class MemoryWatcherExtensions
        {
            /// <summary>
            /// Gets the maximun consumed memory.
            /// </summary>
            /// <param name="memory">An instance of memory wacher implementation</param>
            /// <returns>The maximun consumed memory during the memory performance evaluation.</returns>
            /// <seealso cref="IMemoryWatcher"/>
            public static long GetMaxMemory(this IMemoryWatcher memory) => memory.GetMeasuredMemory().DefaultIfEmpty().Max();

            /// <summary>
            /// Gets the mean consumed memory.
            /// </summary>
            /// <param name="memory">An instance of memory wacher implementation.</param>
            /// <returns>The mean consumed memory during the memory performance evaluation.</returns>
            /// <seealso cref="IMemoryWatcher"/>
            /// <exception cref="T:System.OverflowException">Throws when the memory values generates a long overflow.</exception>
            public static long GetMeanMemory(this IMemoryWatcher memory)
            {
                var measuredMemory = memory.GetMeasuredMemory();

                long memoryValuesSummed;

                checked
                {
                    memoryValuesSummed = measuredMemory.Aggregate(0L, (previous, mm) => previous + mm);
                }

                return memoryValuesSummed / measuredMemory.Count;
            }
        }
    }
}