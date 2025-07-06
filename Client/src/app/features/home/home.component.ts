import { Component, inject } from '@angular/core';
import { UploadService } from '../../core/upload.service';
import { concatWith } from 'rxjs';
import { FileInfo } from '../../shared/models/foodInfo';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  fileSelected: File | null = null;
  weightInput: Number | null = null;
  fileInfo: FileInfo | null = null;
  imagePreview: string | null = null;

  private uploadService = inject(UploadService);

  uploadImage() {
    // Logic to handle image upload
    if(!this.fileSelected) {
      console.error("No File Selected.");
      return;
    }

    const formData = new FormData();
    formData.append("imageFile", this.fileSelected);

    console.log("Image upload initiated");
    this.uploadService.uploadImage(formData, this.weightInput).subscribe({
      next: (response) => {
        console.log("Upload Successful.")
        this.updateResponse(response);
      },
      error: err => 
        console.log(err)
    });
  }

  updateResponse(response: FileInfo) {
    console.log(response);
    this.fileInfo = response;
  }

  onFileSelected($event: Event) {
    const target = $event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      this.fileSelected = target.files[0];

      // Create Image Url
      const reader = new FileReader();
      reader.readAsDataURL(this.fileSelected);
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      }
    }
  }

  onWeightChange($event: Event) {
    const target = $event.target as HTMLInputElement;
    this.weightInput = Number(target.value);
  }
}
