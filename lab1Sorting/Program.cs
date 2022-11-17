using System;
using System.Diagnostics;

namespace sortings
{
    class Program
    {
        struct FriendlyK
        {
            public int index = 0;
            public long time = Int32.MaxValue;
            public FriendlyK() { }
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Enter R the number of arrays for quick sort");
            int r1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter N the length of arrays for quick sort");
            int n1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter M the range of numbers of arrays for quick sort");
            int m1 = Convert.ToInt32(Console.ReadLine());

            int k1 = FindOptimalKQuick(r1, n1, m1);
            int[][] array1 = Init(r1, n1, m1);
            
            //Arrays before sorting
            Console.WriteLine("Array before quick sort");

            PrintArrOfArr(array1);

            for (int i = 0; i < r1; i++)
            {
                array1[i] = HybridQuickSort(array1[i], k1);
            }
            Console.WriteLine("Array after quick sort");
            //Arrays after sorting
            PrintArrOfArr(array1);

            Console.WriteLine("Enter R the number of arrays for merge sort");
            int r2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter N the length of arrays for merge sort");
            int n2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter M the range of numbers of arrays for merge sort");
            int m2 = Convert.ToInt32(Console.ReadLine());

            int k2 = FindOptimalKMerge(r2, n2, m2);
            int[][] array2 = Init(r2, n2, m2);

            //Arrays before sorting
            Console.WriteLine("Array before merge sort");

            PrintArrOfArr(array2);

            for (int i = 0; i < r2; i++)
            {
                array2[i] = HybridQuickSort(array2[i], k2);
            }
            Console.WriteLine("Array after merge sort");
            //Arrays after sorting
            PrintArrOfArr(array2);

        }

        private static int FindOptimalKMerge(int r, int n, int m)
        {
            Stopwatch stopWatch = new Stopwatch();
            FriendlyK kek = new FriendlyK();

            int[][] arr = Init(r, n, m);

            for (int i = 0; i <= n; i++)
            {
                stopWatch.Start();
                for (int j = 0; j < r; j++)
                {
                    arr[j] = HybridMergeSort(arr[j], i);
                }
                stopWatch.Stop();
                if (stopWatch.ElapsedTicks < kek.time)
                {
                    kek.time = stopWatch.ElapsedTicks;
                    kek.index = i;
                }

            }
            Console.WriteLine("optimal k for merge sort is: " + kek.index + " time is: " + kek.time);
            return kek.index;
        }


        private static int FindOptimalKQuick(int r, int n, int m)
        {
            Stopwatch stopWatch = new Stopwatch();
            FriendlyK kek = new FriendlyK();

            int[][] arr = Init(r, n, m);

            for (int i = 0; i <= n ;i++)
            {
                stopWatch.Start();
                for (int j = 0; j < r; j++)
                {
                    arr[j] = HybridQuickSort(arr[j], i);
                }
                stopWatch.Stop();
                if (stopWatch.ElapsedTicks < kek.time)
                {
                    kek.time = stopWatch.ElapsedTicks;
                    kek.index = i;
                }

            }
            Console.WriteLine("optimal k for quick sort is: " + kek.index + " time is: " + kek.time);
            return kek.index;
        }

        private static int[][] Init(int r, int n, int m)
        {
            Random rand = new Random();
            int[][] arrays = new int[r][];
            for (int i = 0; i < r; i++)
                arrays[i] = new int[n];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arrays[i][j] = rand.Next(0, m); //(в диапазоне от 0 до m-1)
                }
            }
            return arrays;
        }

        private static void PrintArrOfArr(int[][] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"Array #{i}: {string.Join(", ", arr[i])}");
            }
        }

        private static int[] HybridMergeSort(int[] array, int k)
        {
            int[] MergeSortedPart = array[..(array.Length - k)]; //Если k=4 а массив длины 6 то выведется 0,1 индексы
            int[] InsertionSortedPart = array[(array.Length - k)..]; //Если k=4 а массив длины 6 то выведется 2,3,4,5 индексы
            int[] result = new int[array.Length];

            MergeSort(MergeSortedPart).CopyTo(result, 0);
            InsertionSort(InsertionSortedPart).CopyTo(result, array.Length - k);

            //return result;
            return MergeSort(result);
        }

        private static int[] HybridQuickSort(int[] array, int k)
        {
            int[] QuickSortedPart = array[..(array.Length - k)]; //Если k=4 а массив длины 6 то выведется 0,1 индексы
            int[] InsertionSortedPart = array[(array.Length - k)..]; //Если k=4 а массив длины 6 то выведется 2,3,4,5 индексы
            int[] result = new int[array.Length];

            QuickSort(QuickSortedPart, 0, QuickSortedPart.Length - 1).CopyTo(result, 0);
            InsertionSort(InsertionSortedPart).CopyTo(result, array.Length - k);

            //return result;
            return QuickSort(result, 0, array.Length - 1);
        }

        private static int[] MergeSort(int[] array)
        {
            if(array.Length < 2)
            {
                return array;
            }
            
            int mid = array.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[array.Length - mid];

            left = array[..mid];
            right = array[mid..];

            MergeSort(left);
            MergeSort(right);
            Merge(array, left, right); //склейка подмассивов
            return array;
        }

        private static void Merge(int[] targetArray, int[] array1, int[] array2)
        {
            int array1MinIndex = 0;
            int array2MinIndex = 0;

            int targetArrayMinIndex = 0;

            while (array1MinIndex < array1.Length && array2MinIndex < array2.Length) { //пока не закончится один из подмассивов

                if (array1[array1MinIndex] <= array2[array2MinIndex]) //минимальные элементы уже расставлены, нам надо их просто подставлять по очереди
                {
                    targetArray[targetArrayMinIndex] = array1[array1MinIndex];
                    array1MinIndex++;
                }
                else
                {
                    targetArray[targetArrayMinIndex] = array2[array2MinIndex];
                    array2MinIndex++;
                }
                targetArrayMinIndex++; 
            }
            while (array1MinIndex < array1.Length) //то что осталось в первом массиве переносим в целевой
            {
                targetArray[targetArrayMinIndex] = array1[array1MinIndex];
                array1MinIndex++;
                targetArrayMinIndex++;
            }

            while(array2MinIndex < array2.Length)
            {
                targetArray[targetArrayMinIndex] = array2[array2MinIndex];
                array2MinIndex++;
                targetArrayMinIndex++;
            }
        }
        private static int[] InsertionSort(int[] array)
        {
            int index;
            int currentNumber;

            for (int i = 0; i < array.Length; i++) // в цикле 1 + array.Length-1 сравнения + array.Length-1 инкремент //2a.L - 1
            {
                index = i; //+array.Length // 3a.L - 1
                currentNumber = array[i]; //+2*array.Length // 5a.L - 1

                while (index > 0 && currentNumber < array[index - 1]) //при первой итерации проверим с первым левым числом от текущего, +array.Length*array.Length!(исходя из index пробегает по всем i от 0 до array.Length очень странные мысли) * 4 // (5 + 4a.L!)a.L - 1
                                                                     //при второй итерации это второй от текущего
                {
                    array[index] = array[index - 1]; //+array.Length*array.Length! * 3 //(5 + 7a.L!)a.L - 1
                    index--; //+array.Length*array.Length! //(5 + 8a.L!)a.L - 1
                }
                                              //массив закончился или найдено число которое меньше текущего
                array[index] = currentNumber; //на определенный индекс помещаем текущий элемент +array.Length * 2 //(7 + 8a.L!)a.L - 1

            }
            return array;
        }


        private static int[] QuickSort (int[] array, int minIndex, int maxIndex)
        {
            if(minIndex >= maxIndex)
            {
                return array;
            }

            int pivotIndex = GetPivotIndex(array, minIndex, maxIndex);
            QuickSort(array, minIndex, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, maxIndex);
            return array;
        }

        private static int GetPivotIndex(int[] array, int minIndex, int maxIndex )
        {
            int pivot = minIndex - 1;

            for(int i = minIndex; i < maxIndex; i++) //тут опасно, было <= но с < тож работает
            {
                if (array[i] < array[maxIndex]) //array[end] is pivot
                {
                
                    pivot++; //передвигаем все числа, меньшие опорного числа влево и двигаем пивот
                    Swap(ref array[pivot], ref array[i]);
                }
            }
            //put pivot(array[end]) between left and right subarrays
            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);


            return pivot;
        }

        private static void Swap(ref int leftItem, ref int rightItem)
        {
            int temp = leftItem;
            leftItem = rightItem;
            rightItem = temp;
        }
    }
}