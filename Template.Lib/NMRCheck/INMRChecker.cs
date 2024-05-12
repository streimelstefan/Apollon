﻿using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.NMRCheck;

public interface INMRChecker
{
    CheckRule[] GenerateNMRCheckRules(PreprocessedStatement[] preprocessedStatements);
}
