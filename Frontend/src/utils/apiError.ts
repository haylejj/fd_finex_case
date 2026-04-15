export function extractErrorMessage(payload: unknown, fallback: string) {
  if (!payload || typeof payload !== 'object') {
    return fallback
  }

  const typedPayload = payload as {
    errorList?: string[]
    errors?: Record<string, string[]>
    title?: string
  }

  if (typedPayload.errorList && typedPayload.errorList.length > 0) {
    return typedPayload.errorList.join(', ')
  }

  if (typedPayload.errors) {
    const messages = Object.values(typedPayload.errors).flat()
    if (messages.length > 0) {
      return messages.join(', ')
    }
  }

  if (typedPayload.title) {
    return typedPayload.title
  }

  return fallback
}
