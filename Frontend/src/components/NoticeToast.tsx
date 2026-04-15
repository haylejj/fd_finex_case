import type { Notice } from '../types/todo'

type NoticeToastProps = {
  notice: Notice | null
}

function NoticeToast({ notice }: NoticeToastProps) {
  if (!notice) {
    return null
  }

  return <div className={`toast ${notice.type}`}>{notice.message}</div>
}

export default NoticeToast
