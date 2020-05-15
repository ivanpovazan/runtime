#include "pdbstream.h"

PdbStream::~PdbStream()
{
    if (m_data != NULL) delete[] m_data;
}

__checkReturn
HRESULT PdbStream::SetData(PORTABLE_PDB_STREAM* data)
{
    HRESULT hr = S_OK;

    m_size = sizeof(data->id) +
        sizeof(data->entryPoint) +
        sizeof(data->referencedTypeSystemTables) +
        sizeof(ULONG) * data->typeSystemTableRowsSize;
    m_data = new BYTE[m_size];

    ULONG offset = 0;
    IfFailGo(hr = memcpy_s(m_data + offset, m_size, data->id, sizeof(data->id)));
    offset += sizeof(data->id);
    IfFailGo(hr = memcpy_s(m_data + offset, m_size, &data->entryPoint, sizeof(data->entryPoint)));
    offset += sizeof(data->entryPoint);
    IfFailGo(hr = memcpy_s(m_data + offset, m_size, &data->referencedTypeSystemTables, sizeof(data->referencedTypeSystemTables)));
    offset += sizeof(data->referencedTypeSystemTables);
    IfFailGo(hr = memcpy_s(m_data + offset, m_size, data->typeSystemTableRows, sizeof(ULONG) * data->typeSystemTableRowsSize));
    offset += sizeof(ULONG) * data->typeSystemTableRowsSize;

    _ASSERTE(offset == m_size);
ErrExit:
    return hr;
}

__checkReturn
HRESULT PdbStream::SaveToStream(IStream* stream)
{
    HRESULT hr = S_OK;
    if (!IsEmpty())
    {
        ULONG written = 0;
        hr = stream->Write(m_data, m_size, &written);
        _ASSERTE(m_size == written);
    }
    return hr;
}

bool PdbStream::IsEmpty()
{
    return m_size == 0;
}

ULONG PdbStream::GetSize()
{
    return m_size;
}
