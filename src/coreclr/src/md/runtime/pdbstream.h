#ifndef _PDBSTREAM_H_
#define _PDBSTREAM_H_

#if _MSC_VER >= 1100
# pragma once
#endif

#include "metamodel.h"

class PdbStream
{
private:
    BYTE* m_data;
    ULONG m_size;
public:
    PdbStream() : m_data(NULL), m_size(0) {}
    ~PdbStream();

    __checkReturn
        HRESULT SetData(PORTABLE_PDB_STREAM* data);

    __checkReturn
        HRESULT SaveToStream(IStream* stream);

    bool IsEmpty();

    ULONG GetSize();
};

#endif
