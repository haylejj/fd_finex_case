import type { FormEvent } from 'react'
import type { ModalType, TodoItem } from '../types/todo'

type TodoModalProps = {
  activeModal: ModalType
  selectedTodo: TodoItem | null
  titleInput: string
  modalError: string | null
  saving: boolean
  onTitleChange: (value: string) => void
  onClose: () => void
  onSubmitCreateOrUpdate: (event: FormEvent<HTMLFormElement>) => void
  onConfirmDelete: () => void
  onConfirmToggle: () => void
}

function TodoModal({
  activeModal,
  selectedTodo,
  titleInput,
  modalError,
  saving,
  onTitleChange,
  onClose,
  onSubmitCreateOrUpdate,
  onConfirmDelete,
  onConfirmToggle,
}: TodoModalProps) {
  if (activeModal === 'none') {
    return null
  }

  return (
    <div className="modal-backdrop">
      <div className="modal">
        {(activeModal === 'create' || activeModal === 'update') && (
          <>
            <h3>{activeModal === 'create' ? 'Yeni Görev Ekle' : 'Görev Güncelle'}</h3>
            <form onSubmit={onSubmitCreateOrUpdate} className="modal-form">
              <input
                value={titleInput}
                onChange={(event) => onTitleChange(event.target.value)}
                maxLength={200}
                placeholder="Görev başlığı"
              />
              {modalError && <p className="modal-error">{modalError}</p>}
              <div className="modal-actions">
                <button type="button" onClick={onClose}>
                  İptal
                </button>
                <button type="submit" className="primary" disabled={saving}>
                  {saving ? 'Kaydediliyor...' : 'Kaydet'}
                </button>
              </div>
            </form>
          </>
        )}

        {activeModal === 'delete' && (
          <>
            <h3>Görevi Sil</h3>
            <p>Bu görevi silmek istediğine emin misin?</p>
            <div className="modal-actions">
              <button onClick={onClose}>İptal</button>
              <button className="danger" onClick={onConfirmDelete} disabled={saving}>
                {saving ? 'Siliniyor...' : 'Evet, Sil'}
              </button>
            </div>
          </>
        )}

        {activeModal === 'toggle' && selectedTodo && (
          <>
            <h3>Durum Değiştir</h3>
            <p>
              "{selectedTodo.title}" görevini {selectedTodo.isCompleted ? 'bekliyor' : 'tamamlandı'} olarak
              işaretlemek istiyor musun?
            </p>
            <div className="modal-actions">
              <button onClick={onClose}>İptal</button>
              <button className="primary" onClick={onConfirmToggle} disabled={saving}>
                {saving ? 'Güncelleniyor...' : 'Evet'}
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  )
}

export default TodoModal
