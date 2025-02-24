import { AxiosError } from 'axios'

export const handleError = (e: AxiosError)  => {
  if (e instanceof AxiosError && typeof e.response !== 'undefined') {
    var err = e.response.data as string[]
    return err.length > 0 ? err : e.response?.data.errors
  }
}
