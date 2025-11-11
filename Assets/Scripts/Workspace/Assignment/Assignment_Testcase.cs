using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AssignmentSystem.Services;
using System;

namespace Assignment
{
    public class Assignment_Testcase
    {
        private IAssignment assignment;

        [SetUp]
        public void Setup()
        {
            // Reset static state before each test
            AssignmentDebugConsole.Clear();

            // Use StudentSolution as the test subject
            assignment = new StudentSolution();
        }

        [TearDown]
        public void Teardown()
        {
            if (assignment is MonoBehaviour)
            {
                MonoBehaviour.DestroyImmediate(assignment as MonoBehaviour);
            }
        }

        #region Lecture Tests

        [Category("Lecture")]
        [TestCase(new int[] { 5, 2, 8, 1, 9 }, new int[] { 1, 2, 5, 8, 9 }, TestName = "LCT01_SelectionSortAscending_BasicScenario", Description = "Basic scenario with mixed numbers")]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, TestName = "LCT01_SelectionSortAscending_AlreadySorted", Description = "Already sorted array")]
        [TestCase(new int[] { 5, 4, 3, 2, 1 }, new int[] { 1, 2, 3, 4, 5 }, TestName = "LCT01_SelectionSortAscending_ReverseSorted", Description = "Reverse sorted array")]
        [TestCase(new int[] { 42 }, new int[] { 42 }, TestName = "LCT01_SelectionSortAscending_SingleElement", Description = "Single element array")]
        public void Test_LCT01_SelectionSortAscending_AllCases(int[] numbers, int[] expected)
        {
            assignment.LCT01_SelectionSortAscending(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            string expectedOutput = string.Join("\n", expected);
            TestUtils.AssertMultilineEqual(expectedOutput, output, "Array should be sorted in ascending order using Selection Sort");
        }

        [Category("Lecture")]
        [TestCase(new int[] { 64, 34, 25, 12, 22, 11, 90 }, new int[] { 11, 12, 22, 25, 34, 64, 90 }, TestName = "LCT02_BubbleSortAscending_BasicScenario", Description = "Basic scenario with mixed numbers")]
        [TestCase(new int[] { 3, 1, 3, 2, 1 }, new int[] { 1, 1, 2, 3, 3 }, TestName = "LCT02_BubbleSortAscending_WithDuplicates", Description = "Array with duplicate values")]
        public void Test_LCT02_BubbleSortAscending_AllCases(int[] numbers, int[] expected)
        {
            assignment.LCT02_BubbleSortAscending(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            string expectedOutput = string.Join("\n", expected);
            TestUtils.AssertMultilineEqual(expectedOutput, output, "Array should be sorted in ascending order using Bubble Sort");
        }

        [Category("Lecture")]
        [TestCase(new int[] { 12, 11, 13, 5, 6 }, new int[] { 5, 6, 11, 12, 13 }, TestName = "LCT03_InsertionSortAscending_BasicScenario", Description = "Basic scenario with mixed numbers")]
        [TestCase(new int[] { -5, 2, -3, 8, 0 }, new int[] { -5, -3, 0, 2, 8 }, TestName = "LCT03_InsertionSortAscending_WithNegatives", Description = "Array with negative numbers")]
        public void Test_LCT03_InsertionSortAscending_AllCases(int[] numbers, int[] expected)
        {
            assignment.LCT03_InsertionSortAscending(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            string expectedOutput = string.Join("\n", expected);
            TestUtils.AssertMultilineEqual(expectedOutput, output, "Array should be sorted in ascending order using Insertion Sort");
        }

        #endregion

        #region Assignment Tests

        [Category("Assignment")]
        [TestCase(new int[] { 5, 2, 8, 1, 9 }, new int[] { 9, 8, 5, 2, 1 }, TestName = "AS01_SelectionSortDescending_BasicScenario", Description = "Basic scenario with mixed numbers")]
        [TestCase(new int[] { 7, 3, 7, 1, 3 }, new int[] { 7, 7, 3, 3, 1 }, TestName = "AS01_SelectionSortDescending_WithDuplicates", Description = "Array with duplicate values")]
        public void Test_AS01_SelectionSortDescending_AllCases(int[] numbers, int[] expected)
        {
            assignment.AS01_SelectionSortDescending(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            string expectedOutput = string.Join("\n", expected);
            TestUtils.AssertMultilineEqual(expectedOutput, output, "Array should be sorted in descending order using Selection Sort");
        }

        [Category("Assignment")]
        [TestCase(new int[] { 64, 34, 25, 12, 22, 11, 90 }, new int[] { 90, 64, 34, 25, 22, 12, 11 }, TestName = "AS02_BubbleSortDescending_BasicScenario", Description = "Basic scenario with mixed numbers")]
        [TestCase(new int[] { 9, 7, 5, 3, 1 }, new int[] { 9, 7, 5, 3, 1 }, TestName = "AS02_BubbleSortDescending_AlreadyDescending", Description = "Already descending sorted array")]
        public void Test_AS02_BubbleSortDescending_AllCases(int[] numbers, int[] expected)
        {
            assignment.AS02_BubbleSortDescending(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            string expectedOutput = string.Join("\n", expected);
            TestUtils.AssertMultilineEqual(expectedOutput, output, "Array should be sorted in descending order using Bubble Sort");
        }

        [Category("Assignment")]
        [TestCase(new int[] { 12, 11, 13, 5, 6 }, new int[] { 13, 12, 11, 6, 5 }, TestName = "AS03_InsertionSortDescending_BasicScenario", Description = "Basic scenario with mixed numbers")]
        [TestCase(new int[] { -5, 2, -3, 8, 0 }, new int[] { 8, 2, 0, -3, -5 }, TestName = "AS03_InsertionSortDescending_WithNegatives", Description = "Array with negative numbers")]
        public void Test_AS03_InsertionSortDescending_AllCases(int[] numbers, int[] expected)
        {
            assignment.AS03_InsertionSortDescending(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            string expectedOutput = string.Join("\n", expected);
            TestUtils.AssertMultilineEqual(expectedOutput, output, "Array should be sorted in descending order using Insertion Sort");
        }

        [Category("Assignment")]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, "4", TestName = "AS04_FindTheSecondLargestNumber_BasicScenario", Description = "Basic scenario [1,2,3,4,5]")]
        [TestCase(new int[] { 5, 5, 4, 3, 2 }, "4", TestName = "AS04_FindTheSecondLargestNumber_WithDuplicates", Description = "Array with duplicate largest values")]
        [TestCase(new int[] { 3, 7, 1, 9, 5 }, "7", TestName = "AS04_FindTheSecondLargestNumber_UnorderedArray", Description = "Unordered array")]
        [TestCase(new int[] { -1, -5, 3, 0, 2 }, "2", TestName = "AS04_FindTheSecondLargestNumber_WithNegatives", Description = "Array with negative numbers")]
        [TestCase(new int[] { 10, 5 }, "5", TestName = "AS04_FindTheSecondLargestNumber_TwoElements", Description = "Two-element array")]
        public void Test_AS04_FindTheSecondLargestNumber_AllCases(int[] numbers, string expected)
        {
            assignment.AS04_FindTheSecondLargestNumber(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            TestUtils.AssertMultilineEqual(expected, output, $"Second largest number should be {expected}");
        }

        #endregion

        #region Extra Tests

        [Category("Extra")]
        [TestCase(new int[] { 1, 9, 3, 10, 4, 20, 2 }, "The longest consecutive sequence is: 4", TestName = "EX01_FindLongestConsecutiveSequence_BasicScenario", Description = "Basic scenario [1,9,3,10,4,20,2] → [1,2,3,4]")]
        [TestCase(new int[] { 5, 4, 3, 2, 1 }, "The longest consecutive sequence is: 5", TestName = "EX01_FindLongestConsecutiveSequence_AllConsecutive", Description = "All consecutive numbers")]
        [TestCase(new int[] { 1, 3, 5, 7, 9 }, "The longest consecutive sequence is: 1", TestName = "EX01_FindLongestConsecutiveSequence_NoConsecutive", Description = "No consecutive numbers")]
        [TestCase(new int[] { 1, 2, 2, 3, 4 }, "The longest consecutive sequence is: 4", TestName = "EX01_FindLongestConsecutiveSequence_WithDuplicates", Description = "Array with duplicate values")]
        [TestCase(new int[] { 42 }, "The longest consecutive sequence is: 1", TestName = "EX01_FindLongestConsecutiveSequence_SingleElement", Description = "Single element array")]
        [TestCase(new int[] { 100, 4, 200, 1, 3, 2, 101, 102 }, "The longest consecutive sequence is: 4", TestName = "EX01_FindLongestConsecutiveSequence_MultipleSequences", Description = "Multiple consecutive sequences")]
        [TestCase(new int[] { -2, -1, 0, 1, 2, 5 }, "The longest consecutive sequence is: 5", TestName = "EX01_FindLongestConsecutiveSequence_WithNegatives", Description = "Array with negative numbers")]
        public void Test_EX01_FindLongestConsecutiveSequence_AllCases(int[] numbers, string expected)
        {
            assignment.EX01_FindLongestConsecutiveSequence(numbers);

            string output = AssignmentDebugConsole.GetOutput();
            TestUtils.AssertMultilineEqual(expected, output, $"Expected: {expected}");
        }

        #endregion
    }

    public class TestUtils
    {
        internal static void AssertMultilineEqual(string expected, string actual, string message = null)
        {
            string normExpected = expected.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
            string normActual = actual.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
            if (string.IsNullOrEmpty(message))
            {
                message = $"Expected output:\n{normExpected}\n----\nActual output:\n{normActual}";
            }
            Assert.AreEqual(normExpected, normActual, message);
        }
    }
}