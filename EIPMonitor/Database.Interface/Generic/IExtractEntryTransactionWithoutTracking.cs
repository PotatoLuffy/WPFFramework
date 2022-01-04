﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface.Generic
{
    public interface IExtractEntryTransactionWithoutTracking
    {
        Task<T> ExtractEntry<T>(String sqlText, T @t, IDbConnection dbConnection, IDbTransaction dbTransaction);
    }
}
