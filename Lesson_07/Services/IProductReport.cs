﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_07.Services
{
    /// <summary>
    /// Интерфейс, описывающий механизм генерации отчета по товарам
    /// </summary>
    public interface IProductReport
    {
        string CatalogName { get; set; }
        string CatalogDescription { get; set; }
        DateTime CreationDate { get; set; }
        IEnumerable<(int id, string name, string category, decimal price)> Products { get; set; }
        FileInfo Create(string reportFilePath);
    }
}
