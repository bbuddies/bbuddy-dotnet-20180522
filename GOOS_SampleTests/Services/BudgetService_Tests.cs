﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using GOOS_Sample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOOS_Sample.Models;

namespace GOOS_Sample.Services.Tests
{
    [TestClass()]
    public class BudgetService_Tests
    {
        private DateRangeService _range;
        private List<BudgetViewModel> _budgets;
        private static BudgetService _budgetService;

        private void GivenDateRage(string startDate, string endDate)
        {
             _range = new DateRangeService
            {
                SatrtDate = startDate,
                EndDate = endDate
            };
        }

        private void GivenBudgets(params BudgetViewModel[] budgets)
        {
            _budgets = budgets.ToList();
        }
       
        

        [TestInitialize()]
        public void BeforeTests()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(budgetRepo);
            
        }

        [TestMethod()]
        public void budget_in_may_one_day_will_10()
        {
            GivenDateRage("2018-05-16", "2018-05-16");
            GivenBudgets(
                new BudgetViewModel
                {
                    Amount = 310,
                    Month = "2018-05"
                },
                new BudgetViewModel
                {
                    Amount = 600,
                    Month = "2018-06"
                },
                new BudgetViewModel
                {
                    Amount = 620,
                    Month = "2018-07"
                }
            );
            TotalBudgetShouldBe(10);
        }

        

        [TestMethod()]
        public void budget_in_april_one_day_will_0()
        {
            GivenDateRage("2018-04-16", "2018-04-16");
            GivenBudgets(
                new BudgetViewModel
                {
                    Amount = 310,
                    Month = "2018-05"
                },
                new BudgetViewModel
                {
                    Amount = 600,
                    Month = "2018-06"
                },
                new BudgetViewModel
                {
                    Amount = 620,
                    Month = "2018-07"
                }
            );
            
            TotalBudgetShouldBe(0);
        }

        [TestMethod()]
        public void budget_in_range_will_1050()
        {
            GivenDateRage("2018-05-17", "2018-07-15");
            GivenBudgets(
                new BudgetViewModel
                {
                    Amount = 310,
                    Month = "2018-05"
                },
                new BudgetViewModel
                {
                    Amount = 600,
                    Month = "2018-06"
                },
                new BudgetViewModel
                {
                    Amount = 620,
                    Month = "2018-07"
                }
            );
            TotalBudgetShouldBe(1050);
        }

        [TestMethod()]
        public void budget_in_range_will_460()
        {
            GivenDateRage("2018-05-16", "2018-07-15");
            GivenBudgets(
                new BudgetViewModel
                {
                    Amount = 310,
                    Month = "2018-05"
                },
                new BudgetViewModel
                {
                    Amount = 620,
                    Month = "2018-07"
                }
            );
            TotalBudgetShouldBe(460);
        }

        private void TotalBudgetShouldBe(int result)
        {
            var actual = _budgetService.CalculateRangeTotal(_range, _budgets);

            Assert.AreEqual(result, actual);
        }
    }
}