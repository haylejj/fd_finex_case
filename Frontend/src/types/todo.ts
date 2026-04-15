export type TodoItem = {
  id: number
  title: string
  isCompleted: boolean
  createdAt: string
}

export type ServiceResult<T> = {
  isSuccess: boolean
  errorList?: string[]
  data?: T
  urlAsCreated?: string
}

export type ModalType = 'none' | 'create' | 'update' | 'delete' | 'toggle'
export type NoticeType = 'success' | 'error'

export type Notice = {
  type: NoticeType
  message: string
}
