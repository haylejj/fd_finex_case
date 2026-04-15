import type { TodoItem } from '../types/todo'

type TodoListPanelProps = {
  todos: TodoItem[]
  selectedTodoId: number | null
  loading: boolean
  onRefresh: () => void
  onSelectTodo: (todoId: number) => void
}

function TodoListPanel({
  todos,
  selectedTodoId,
  loading,
  onRefresh,
  onSelectTodo,
}: TodoListPanelProps) {
  return (
    <section className="panel list-panel">
      <div className="panel-header">
        <h1>Mini Görev Yönetimi</h1>
        <button onClick={onRefresh} disabled={loading}>
          Yenile
        </button>
      </div>

      {loading ? (
        <p>Yükleniyor...</p>
      ) : (
        <ul className="todo-list">
          {todos.map((todo) => (
            <li
              key={todo.id}
              className={selectedTodoId === todo.id ? 'selected' : ''}
              onClick={() => onSelectTodo(todo.id)}
            >
              <div className="todo-title-row">
                <span className={todo.isCompleted ? 'completed' : ''}>{todo.title}</span>
                <span className={`badge ${todo.isCompleted ? 'ok' : 'wait'}`}>
                  {todo.isCompleted ? 'Tamamlandı' : 'Bekliyor'}
                </span>
              </div>
            </li>
          ))}
        </ul>
      )}
    </section>
  )
}

export default TodoListPanel
