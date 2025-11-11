using UnityEngine;
using System.Collections.Generic;

namespace Assignment
{

    public interface IAssignment
    {
        #region Lecture 
        /// <summary>
        /// เรียงลำดับตัวเลขจากน้อยไปมากโดยใช้ Selection Sort
        /// </summary>
        /// <param name="numbers"></param>
        public void LCT01_SelectionSortAscending(int[] numbers);

        /// <summary>
        /// เรียงลำดับตัวเลขจากน้อยไปมากโดยใช้ Bubble Sort
        /// </summary>
        /// <param name="numbers"></param>
        public void LCT02_BubbleSortAscending(int[] numbers);

        /// <summary>
        /// เรียงลำดับตัวเลขจากน้อยไปมากโดยใช้ Insertion Sort
        /// </summary>
        /// <param name="numbers"></param>
        public void LCT03_InsertionSortAscending(int[] numbers);

        #endregion

        #region Assignment

        /// <summary>
        /// เรียงลำดับตัวเลขจากมากไปน้อยโดยใช้ Selection Sort
        /// </summary>
        /// <param name="numbers"></param>
        public void AS01_SelectionSortDescending(int[] numbers);

        /// <summary>
        /// เรียงลำดับตัวเลขจากมากไปน้อยโดยใช้ Bubble Sort
        /// </summary>
        /// <param name="numbers"></param>
        public void AS02_BubbleSortDescending(int[] numbers);

        /// <summary>
        /// เรียงลำดับตัวเลขจากมากไปน้อยโดยใช้ Insertion Sort
        /// </summary>
        /// <param name="numbers"></param>
        public void AS03_InsertionSortDescending(int[] numbers);

        /// <summary>
        /// ค้นหาตัวเลขที่มีค่ามากเป็นอันดับสองใน array
        /// ให้เขียนโปรแกรมเพื่อค้นหาตัวเลขที่มีค่ามากเป็นอันดับสองจาก array ที่ได้รับเป็น input
        /// เช่น input ที่ได้รับมาคือ[1 2 3 4 5] ตัวเลขที่มีค่ามากเป็นอันดับสองคือ 4
        /// </summary>
        /// <param name="numbers"></param>
        public void AS04_FindTheSecondLargestNumber(int[] numbers);

        #endregion

        #region Extra

        /// <summary>
        /// ค้นหาความยาวชุดตัวเลขที่เรียงลำดับติดต่อกันที่ยาวที่สุด
        /// ให้เขียนโปรแกรมเพื่อค้นหาความยาวของชุดตัวเลขที่เรียงลำดับติดต่อกันที่ยาวที่สุดจาก array ที่ได้รับเป็น input
        /// ตัวอย่างความยาวชุดตัวเลขที่เรียงลำดับติดต่อกันที่ยาวที่สุด เช่น input ที่ได้รับมาคือ[1 9 3 10 4 20 2] เมื่อนำมาเรียงจากน้อยไปมากแล้วจะได้เป็น[1 2 3 4 9 10 20]
        /// ซึ่งเราจะได้ชุดตัวเลขย่อยๆที่เรียง 3 ชุดคือ
        /// [1 2 3 4]
        /// [9 10]
        /// [20]
        /// จะเห็นว่า[1 2 3 4] มีความยาวเท่ากับ4 ซึ่งเป็นชุดตัวเลขที่ยาวที่สุดเมื่อเทียบกับ 2 ชุดที่เหลือ
        /// ดังนั้นเราสามารถบอกได้ว่าชุดตัวเลขนี้[1 9 3 10 4 20 2] มี longest consecutive sequence ความยาวเท่ากับ 4
        /// </summary>
        /// <param name="numbers"></param>
        public void EX01_FindLongestConsecutiveSequence(int[] numbers);

        #endregion 
    }
}
