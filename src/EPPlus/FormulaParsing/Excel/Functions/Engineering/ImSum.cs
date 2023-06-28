﻿/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  21/06/2023         EPPlus Software AB       Initial release EPPlus 7
 *************************************************************************************************/
using OfficeOpenXml.FormulaParsing.Excel.Functions.MathFunctions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Metadata;
using OfficeOpenXml.FormulaParsing.FormulaExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering
{
    internal class ImSum : ImFunctionBase
    {
        public override CompileResult Execute(IList<FunctionArgument> arguments, ParsingContext context)
        {
            var argumentStrings = arguments[0].Value.ToString().Split(',');
            var realPart = 0d;
            var imagPart = 0d;
            var imSuffix = string.Empty;

            foreach (var argument in argumentStrings )
            {
                GetComplexNumbers(argument, out double real, out double imag, out string imaginarySuffix);
                if (double.IsNaN(real) || double.IsNaN(imag))
                {
                    return CompileResult.GetErrorResult(eErrorType.Num);
                }
                if (!string.IsNullOrEmpty(imaginarySuffix))
                {
                    if (!string.IsNullOrEmpty(imSuffix) && imSuffix != imaginarySuffix)
                    {
                        return CompileResult.GetErrorResult(eErrorType.Value);
                    }
                    imSuffix = imaginarySuffix;
                }
                realPart += real;
                imagPart += imag;
            }

            if (imagPart == 0)
            {
                return CreateResult(realPart.ToString(), DataType.String);
            }
            var sign = (imagPart < 0) ? "-" : "+";
            var result = string.Format("{0}{1}{2}{3}", realPart, sign, Math.Abs(imagPart), imSuffix);
            return CreateResult(result, DataType.String);
        }
    }
}