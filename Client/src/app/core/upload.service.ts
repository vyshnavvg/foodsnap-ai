import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { FileInfo } from '../shared/models/foodInfo';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  private apiUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);

  uploadImage(file: FormData, weight: Number | null) {
    return this.httpClient.post<FileInfo>(`${this.apiUrl}food/upload-image?weight=${weight}`, file);
  }
}
