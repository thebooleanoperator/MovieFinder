import axios from "axios"

export class ApiService {
  private baseUrl = window.location.protocol + '//' + window.location.host;

  public Post = async (url: string, data?: any) => {
    return await axios.post(`${this.baseUrl}${url}`, data)
  }
}