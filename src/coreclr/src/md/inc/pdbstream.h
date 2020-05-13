#ifndef _PDBSTREAM_H_
#define _PDBSTREAM_H_

#if _MSC_VER >= 1100
# pragma once
#endif

#include <basetsd.h>
#include <string.h>
#include "metamodel.h"
#include "table.h"

class PdbStream {

private:
    UINT8               m_Id[20];
    mdMethodDef         m_EntryPoint;
    UINT64              m_ReferencedTypeSystemTables;
    UINT32*             m_TypeSystemTableRows;
    UINT32              m_TypeSystemTableRowsSize;

    UINT64              GetReferencedTypeSystemTables();
    void                SetReferencedTypeSystemTables(UINT64 referencedTypeSystemTables);

    UINT32*             GetTypeSystemTableRows();
    void                SetTypeSystemTableRows(UINT32* typeSystemTableRows, UINT32 size);
    void                SetTypeSystemTableRowsSize(UINT32 size);

public:
    PdbStream();
    ~PdbStream();

    UINT8*              GetId();
    void                SetId(UINT8* id, UINT32 byteSize);

    mdMethodDef         GetEntryPoint();
    void                SetEntryPoint(mdMethodDef entryPoint);

    void                SetupTableReferences(MetaData::TableRW tables[TBL_COUNT]);

    bool                IsEmpty();
    void                GetByteSize(UINT32* byteSize);
    void                SaveToStream(IStream* stream);
};

#endif
