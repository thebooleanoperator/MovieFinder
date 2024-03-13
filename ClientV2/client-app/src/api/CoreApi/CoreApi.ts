import axios from "axios"

export class ApiService {
  private baseUrl = window.location.protocol + '//' + window.location.host;

  public Post = async (url: string, data?: any) => {
    try {
      return await axios.post(`${this.baseUrl}${url}`, data)
    }
    catch (error) {
      console.log('real error', error)
    }
  }
}