using SistemaMundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaMundoNovo.DAL
{
    public class Singleton
    {
        private static readonly Singleton instance = new Singleton();
        private readonly ApplicationDbContext context;

        private Singleton()
        {
            context = new ApplicationDbContext();
        }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }

        public ApplicationDbContext Context
        {
            get
            {
                return context;
            }
        }
    }
}