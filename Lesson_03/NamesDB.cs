using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_03
{
    public class NamesDB
    {
        public List<string> MNames = new List<string>{"Петр", "Сергей", "Антон", "Евгений", "Павел", "Владимир", "Иван", "Юрий", "Анатолий", "Дмитрий"};
        public List<string> MFirstNames = new List<string> { "Кузнецов", "Петров", "Смольников", "Праведников", "Сотников", "Ким", "Кормильцев", "Садков", "Махов", "Стеблов" };
        public List<string> MLastNames = new List<string> { "Петрович", "Сергеевич", "Антонович", "Евгеньевич", "Павлович", "Владимирович", "Иванович", "Юрьевич", "Анатольевич", "Дмитриевич" };
        public List<string> FNames = new List<string> { "Катерина", "Элина", "Мария", "Анна", "Маргарита", "Светлана", "Анастасия", "Ирина", "Татьяна", "Раиса" };
        public List<string> FFirstNames = new List<string> { "Кузнецова", "Петрова", "Смольникова", "Праведникова", "Сотникова", "Ким", "Кормильцева", "Садкова", "Махова", "Стеблова" };
        public List<string> FLastNames = new List<string> { "Петровна", "Сергеевна", "Антоновна", "Евгеньевна", "Павловна", "Владимировна", "Ивановна", "Юрьевна", "Анатольевна", "Дмитриевна" };
    }
}
