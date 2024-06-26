/* libunwind - a platform-independent unwind library
   Copyright (C) 2008 CodeSourcery
   Copyright (C) 2021 Zhaofeng Li

This file is part of libunwind.

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.  */

#include "unwind_i.h"

int
unw_get_save_loc (unw_cursor_t *cursor, int reg, unw_save_loc_t *sloc)
{
  struct cursor *c = (struct cursor *) cursor;
  dwarf_loc_t loc;

  switch (reg)
    {
    case UNW_RISCV_X1:
    case UNW_RISCV_X2:
    case UNW_RISCV_X3:
    case UNW_RISCV_X4:
    case UNW_RISCV_X5:
    case UNW_RISCV_X6:
    case UNW_RISCV_X7:
    case UNW_RISCV_X8:
    case UNW_RISCV_X9:
    case UNW_RISCV_X10:
    case UNW_RISCV_X11:
    case UNW_RISCV_X12:
    case UNW_RISCV_X13:
    case UNW_RISCV_X14:
    case UNW_RISCV_X15:
    case UNW_RISCV_X16:
    case UNW_RISCV_X17:
    case UNW_RISCV_X18:
    case UNW_RISCV_X19:
    case UNW_RISCV_X20:
    case UNW_RISCV_X21:
    case UNW_RISCV_X22:
    case UNW_RISCV_X23:
    case UNW_RISCV_X24:
    case UNW_RISCV_X25:
    case UNW_RISCV_X26:
    case UNW_RISCV_X27:
    case UNW_RISCV_X28:
    case UNW_RISCV_X29:
    case UNW_RISCV_X30:
    case UNW_RISCV_X31:
    case UNW_RISCV_PC:
      loc = c->dwarf.loc[reg - UNW_RISCV_X0];
      break;

    default:
      loc = DWARF_NULL_LOC;       /* default to "not saved" */
      break;
    }

  memset (sloc, 0, sizeof (*sloc));

  if (DWARF_IS_NULL_LOC (loc))
    {
      sloc->type = UNW_SLT_NONE;
      return 0;
    }

#if !defined(UNW_LOCAL_ONLY)
  if (DWARF_IS_REG_LOC (loc))
    {
      sloc->type = UNW_SLT_REG;
      sloc->u.regnum = DWARF_GET_REG_LOC (loc);
    }
  else
#endif
    {
      sloc->type = UNW_SLT_MEMORY;
      sloc->u.addr = DWARF_GET_MEM_LOC (loc);
    }
  return 0;
}
