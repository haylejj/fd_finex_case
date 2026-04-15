import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import NoticeToast from './components/NoticeToast'
import TodoActionsPanel from './components/TodoActionsPanel'
import TodoListPanel from './components/TodoListPanel'
import TodoModal from './components/TodoModal'
import type { ModalType, Notice, NoticeType, ServiceResult, TodoItem } from './types/todo'
import { extractErrorMessage } from './utils/apiError'

const API_BASE_URL = 'https://localhost:7174/api/todos'

function App() {
  const [todos, setTodos] = useState<TodoItem[]>([])
  const [titleInput, setTitleInput] = useState('')
  const [loading, setLoading] = useState(false)
  const [modalError, setModalError] = useState<string | null>(null)
  const [notice, setNotice] = useState<Notice | null>(null)
  const [selectedTodoId, setSelectedTodoId] = useState<number | null>(null)
  const [activeModal, setActiveModal] = useState<ModalType>('none')
  const [saving, setSaving] = useState(false)

  useEffect(() => {
    void loadTodos()
  }, [])

  const selectedTodo = useMemo(
    () => todos.find((todo) => todo.id === selectedTodoId) ?? null,
    [todos, selectedTodoId],
  )

  useEffect(() => {
    if (!notice) {
      return
    }

    const timer = setTimeout(() => {
      setNotice(null)
    }, 2500)

    return () => clearTimeout(timer)
  }, [notice])

  function showNotice(type: NoticeType, message: string) {
    setNotice({ type, message })
  }

  async function loadTodos() {
    setLoading(true)

    try {
      const response = await fetch(API_BASE_URL)
      const payload = await response.json().catch(() => null)
      const result = payload as ServiceResult<TodoItem[]> | null

      if (!response.ok || !result?.isSuccess) {
        showNotice('error', extractErrorMessage(payload, 'Görevler yüklenemedi.'))
        return
      }

      const items = result.data ?? []
      setTodos(items)
      if (items.length > 0 && !items.some((item) => item.id === selectedTodoId)) {
        setSelectedTodoId(items[0].id)
      }
    } catch {
      showNotice('error', 'API bağlantısı kurulamadı.')
    } finally {
      setLoading(false)
    }
  }

  function openCreateModal() {
    setTitleInput('')
    setModalError(null)
    setActiveModal('create')
  }

  function openUpdateModal() {
    if (!selectedTodo) {
      return
    }
    setTitleInput(selectedTodo.title)
    setModalError(null)
    setActiveModal('update')
  }

  async function handleCreateOrUpdate(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!titleInput.trim()) {
      setModalError('Başlık boş olamaz.')
      return
    }

    const isCreate = activeModal === 'create'
    if (!isCreate && !selectedTodo) {
      setModalError('Güncellenecek görev bulunamadı.')
      return
    }

    const currentTodo = selectedTodo
    const method = isCreate ? 'POST' : 'PUT'
    const body = isCreate
      ? { title: titleInput }
      : { id: currentTodo!.id, title: titleInput, isCompleted: currentTodo!.isCompleted }

    setSaving(true)
    setModalError(null)

    try {
      const response = await fetch(API_BASE_URL, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
      })

      const payload = await response.json().catch(() => null)
      const result = payload as ServiceResult<TodoItem> | null
      if (!response.ok || !result?.isSuccess || !result.data) {
        setModalError(extractErrorMessage(payload, 'İşlem başarısız.'))
        return
      }

      if (isCreate) {
        setTodos((current) => [result.data!, ...current])
      } else {
        setTodos((current) =>
          current.map((item) => (item.id === result.data!.id ? result.data! : item)),
        )
      }

      setSelectedTodoId(result.data.id)
      setActiveModal('none')
      setTitleInput('')
      setModalError(null)
      showNotice('success', isCreate ? 'Görev başarıyla eklendi.' : 'Görev başarıyla güncellendi.')
    } catch {
      setModalError('İşlem sırasında hata oluştu.')
    } finally {
      setSaving(false)
    }
  }

  async function confirmToggle() {
    if (!selectedTodo) {
      return
    }

    setSaving(true)

    try {
      const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          id: selectedTodo.id,
          title: selectedTodo.title,
          isCompleted: !selectedTodo.isCompleted,
        }),
      })

      const payload = await response.json().catch(() => null)
      const result = payload as ServiceResult<TodoItem> | null
      if (!response.ok || !result?.isSuccess || !result.data) {
        showNotice('error', extractErrorMessage(payload, 'Görev güncellenemedi.'))
        return
      }

      setTodos((current) =>
        current.map((item) => (item.id === result.data!.id ? result.data! : item)),
      )
      setSelectedTodoId(result.data.id)
      setActiveModal('none')
      showNotice('success', 'Durum başarıyla güncellendi.')
    } catch {
      showNotice('error', 'Görev güncellenirken hata oluştu.')
    } finally {
      setSaving(false)
    }
  }

  async function confirmDelete() {
    if (!selectedTodo) {
      return
    }

    setSaving(true)

    try {
      const response = await fetch(`${API_BASE_URL}/${selectedTodo.id}`, {
        method: 'DELETE',
      })

      if (!response.ok && response.status !== 204) {
        const payload = await response.json().catch(() => null)
        const message = extractErrorMessage(payload, 'Görev silinemedi.')
        showNotice('error', message)
        return
      }

      const nextTodos = todos.filter((item) => item.id !== selectedTodo.id)
      setTodos(nextTodos)
      setSelectedTodoId(nextTodos[0]?.id ?? null)
      setActiveModal('none')
      showNotice('success', 'Görev başarıyla silindi.')
    } catch {
      showNotice('error', 'Görev silinirken hata oluştu.')
    } finally {
      setSaving(false)
    }
  }

  return (
    <>
      <div className="layout">
        <TodoListPanel
          todos={todos}
          selectedTodoId={selectedTodoId}
          loading={loading}
          onRefresh={() => void loadTodos()}
          onSelectTodo={setSelectedTodoId}
        />

        <TodoActionsPanel
          selectedTodo={selectedTodo}
          onOpenCreate={openCreateModal}
          onOpenUpdate={openUpdateModal}
          onOpenToggle={() => setActiveModal('toggle')}
          onOpenDelete={() => setActiveModal('delete')}
        />
      </div>

      <TodoModal
        activeModal={activeModal}
        selectedTodo={selectedTodo}
        titleInput={titleInput}
        modalError={modalError}
        saving={saving}
        onTitleChange={setTitleInput}
        onClose={() => setActiveModal('none')}
        onSubmitCreateOrUpdate={handleCreateOrUpdate}
        onConfirmDelete={() => void confirmDelete()}
        onConfirmToggle={() => void confirmToggle()}
      />

      <NoticeToast notice={notice} />
    </>
  )
}

export default App
