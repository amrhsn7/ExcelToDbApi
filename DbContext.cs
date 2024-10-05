using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExcelUploadApi
{
    public class ExcelUploaderDbContext : DbContext
    {
        public DbSet<ExcelUploaderModel> Data { get; set; }

        public ExcelUploaderDbContext(DbContextOptions<ExcelUploaderDbContext> options) : base(options) { }
    }


}
