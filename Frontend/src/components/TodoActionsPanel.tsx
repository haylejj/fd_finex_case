import type { TodoItem } from '../types/todo'
import { formatDateTime } from '../utils/date'

type TodoActionsPanelProps = {
  selectedTodo: TodoItem | null
  onOpenCreate: () => void
  onOpenUpdate: () => void
  onOpenToggle: () => void
  onOpenDelete: () => void
}

function TodoActionsPanel({
  selectedTodo,
  onOpenCreate,
  onOpenUpdate,
  onOpenToggle,
  onOpenDelete,
}: TodoActionsPanelProps) {
  return (
    <section className="panel action-panel">
      <h2>İşlemler</h2>
      <button className="primary" onClick={onOpenCreate}>
        Yeni Görev Ekle
      </button>

      <button onClick={onOpenUpdate} disabled={!selectedTodo}>
        Güncelle
      </button>

      <button onClick={onOpenToggle} disabled={!selectedTodo}>
        Tamamlandı Durumu Değiştir
      </button>

      <button className="danger" onClick={onOpenDelete} disabled={!selectedTodo}>
        Sil
      </button>

      {selectedTodo ? (
        <div className="selected-detail">
          <h3>Seçili Görev</h3>
          <p>
            <strong>Başlık:</strong> {selectedTodo.title}
          </p>
          <p>
            <strong>Durum:</strong> {selectedTodo.isCompleted ? 'Tamamlandı' : 'Bekliyor'}
          </p>
          <p>
            <strong>Oluşturulma:</strong> {formatDateTime(selectedTodo.createdAt)}
          </p>
        </div>
      ) : (
        <p>İşlem yapmak için soldan bir görev seç.</p>
      )}
    </section>
  )
}

export default TodoActionsPanel
