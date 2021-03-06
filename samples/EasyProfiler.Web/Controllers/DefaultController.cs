﻿using EasyProfiler.SQLServer.Abstractions;
using EasyProfiler.SQLServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyProfiler.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        private readonly SampleDbContext sampleDbContext;

        public DefaultController(SampleDbContext sampleDbContext)
        {
            this.sampleDbContext = sampleDbContext;
        }

        /// <summary>
        /// Insert Customers
        /// </summary>
        /// <returns>
        /// NoContent
        /// </returns>
        [HttpPost("InsertCustomers")]
        [NonAction]
        public async Task<IActionResult> InsertCustomerAsync()
        {
            for (int i = 0; i < 10000; i++)
            {
                await sampleDbContext.Customers.AddAsync(new Customer
                {
                    Name = "Customer Name " + i,
                    Surname = "Customer Surname " +i,
                    CreateDate = DateTime.UtcNow
                });
            }
            await sampleDbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Get All Customers.
        /// </summary>
        /// <returns>
        /// List of customers.
        /// </returns>
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            return Ok(await sampleDbContext.Customers.ToListAsync());
        }

        /// <summary>
        /// Advanced filter for easy profiler.
        /// </summary>
        /// <param name="model">
        /// Advanced filter model.
        /// </param>
        /// <param name="easyProfilerService">
        /// Easy profiler service.
        /// </param>
        /// <returns>
        /// List of profiler.
        /// </returns>
        [HttpGet("AdvancedFilterForEasyProfiler")]
        public async Task<IActionResult> AdvancedFilterForEasyProfilerAsync([FromQuery] AdvancedFilterModel model,[FromServices] IEasyProfilerService easyProfilerService)
        {
            return Ok(await easyProfilerService.AdvancedFilterAsync(model));
        }
    }
}
