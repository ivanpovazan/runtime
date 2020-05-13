#include "pdbstream.h"

PdbStream::PdbStream()
{
    memset(m_Id, 0, sizeof(m_Id));
    m_EntryPoint = mdMethodDefNil;
    m_ReferencedTypeSystemTables = 0L;
    m_TypeSystemTableRows = NULL;
    m_TypeSystemTableRowsSize = 0;
}

PdbStream::~PdbStream()
{
    if (m_TypeSystemTableRows != NULL)
    {
        delete[] m_TypeSystemTableRows;
        m_TypeSystemTableRows = NULL;
        m_TypeSystemTableRowsSize = 0;
    }
}

UINT8* PdbStream::GetId()
{
    return m_Id;
}

void PdbStream::SetId(UINT8* id, UINT32 byteSize)
{
    _ASSERTE(sizeof(m_Id) >= byteSize);
    memcpy_s(m_Id, sizeof(m_Id), id, byteSize);
}

mdMethodDef PdbStream::GetEntryPoint()
{
    return m_EntryPoint;
}

void PdbStream::SetEntryPoint(mdMethodDef entryPoint)
{
    m_EntryPoint = entryPoint;
}

UINT64 PdbStream::GetReferencedTypeSystemTables()
{
    return m_ReferencedTypeSystemTables;
}

void PdbStream::SetReferencedTypeSystemTables(UINT64 referencedTypeSystemTables)
{
    m_ReferencedTypeSystemTables = referencedTypeSystemTables;
}

UINT32* PdbStream::GetTypeSystemTableRows()
{
    return m_TypeSystemTableRows;
}

void PdbStream::SetTypeSystemTableRows(UINT32* typeSystemTableRows, UINT32 size)
{
    SetTypeSystemTableRowsSize(size);
    memcpy_s(m_TypeSystemTableRows, size, typeSystemTableRows, size);
}

void PdbStream::SetTypeSystemTableRowsSize(UINT32 size)
{
    m_TypeSystemTableRowsSize = size;
    m_TypeSystemTableRows = (UINT32*)malloc(sizeof(UINT32) * size);
}

void PdbStream::SetupTableReferences(MetaData::TableRW tables[TBL_COUNT])
{
    UINT count = 0;
    for (UINT i = 0; i < TBL_COUNT; i++)
    {
        if (tables[i].GetRecordCount() > 0)
        {
            m_ReferencedTypeSystemTables |= (UINT64)1UL << i;
            count++;
        }
    }

    SetTypeSystemTableRowsSize(count);
    UINT* ptr = m_TypeSystemTableRows;
    for (UINT i = 0; i < TBL_COUNT; i++)
    {
        UINT rowsSize = tables[i].GetRecordCount();
        if (rowsSize > 0)
        {
            *ptr++ = rowsSize;
        }
    }
}

bool PdbStream::IsEmpty()
{
    return m_TypeSystemTableRowsSize == 0;
}

void PdbStream::GetByteSize(UINT32* byteSize)
{
    if (IsEmpty())
        *byteSize = 0;
    else
        *byteSize = sizeof(m_Id) + sizeof(m_EntryPoint) + sizeof(m_ReferencedTypeSystemTables) + sizeof(UINT32) * m_TypeSystemTableRowsSize;
}

void PdbStream::SaveToStream(IStream* stream)
{
    if (!IsEmpty())
    {
        UINT32 totalSize = 0;
        GetByteSize(&totalSize);
        ULONG written = 0;
        ULONG writtenTotal = 0;

        stream->Write(&m_Id,                            sizeof(m_Id),                                               &written);
        writtenTotal += written;
        stream->Write(&m_EntryPoint,                    sizeof(m_EntryPoint),                                       &written);
        writtenTotal += written;
        stream->Write(&m_ReferencedTypeSystemTables,    sizeof(m_ReferencedTypeSystemTables),                       &written);
        writtenTotal += written;
        stream->Write(m_TypeSystemTableRows,            sizeof(UINT32) * m_TypeSystemTableRowsSize,                 &written);
        writtenTotal += written;

        _ASSERTE(totalSize == writtenTotal);
    }
}
