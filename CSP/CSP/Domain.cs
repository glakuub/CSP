﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.CSP
{
    class Domain<T>
    {
        public T[] Values { private set; get; }
        //public bool[] Used { set; get; }
        //private int _currentIndex ;
        public T Default { set; get; } 

        public int Size { get { return Values.Length; } }

        public Domain(T[] domainArray)
        {
            Values = domainArray;
            //Used = new bool[domainArray.Length];
            
        }
        public Domain(Domain<T> domain)
        {
            Values = domain.Values;
            //Used = new bool[domain.Used.Length];
            //System.Buffer.BlockCopy(domain.Used, 0, Used, 0, domain.Used.Length * sizeof(bool));
            //_currentIndex = domain._currentIndex;
            Default = domain.Default;
        }
        public Domain<T>DeepCopy()
        {
            var result = new Domain<T>(Values);
            //result.Used = new bool[Used.Length];
            //System.Buffer.BlockCopy(Used, 0, result.Used, 0, Used.Length * sizeof(bool));
            //result._currentIndex = _currentIndex;
            return result;
        }
       
        //public bool IsEmpty()
        //{
        //    return Used.All(e => e == true);
        //}
        //public T First()
        //{
        //    return Values[0];
        //}
        //public bool HasNext()
        //{

        //    return !IsEmpty() && _currentIndex  < Values.Length;
        //}
        //public T Next()
        //{
        //    while(Used[_currentIndex])
        //    {
        //        _currentIndex++;
        //    }
        //    T result = Values[_currentIndex];
        //    Used[_currentIndex] = true;
        //    _currentIndex++;
        //    return result;
        //}
        //public void Reset()
        //{
        //    for(int i =0; i<Used.Length;i++)
        //        Used[i] = false;
        //    _currentIndex = 0;
        //}
        public T UnsetValue()
        {
            return Values.Length == 1 ? Values[0] : Default;
        }
        public T[] AsArray()
        {
            return Values;
        }
    }
}
