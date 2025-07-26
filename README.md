FoodSnap AI - A Community driven project part of WeCode Community
![image](https://github.com/user-attachments/assets/cc5c55d0-6eb5-47b7-a223-b057fc125a11)


Demo video: 
https://github.com/user-attachments/assets/5eafcecb-4aff-4e34-a911-2ee615433322

Tech stack used:
Backend: .Net Core Web Api
FrontEnd: Angular
Db: Sql

As part of my journey into exploring AI, I developed FoodSnap AI—a full-stack system that leverages deep learning to identify food items and estimate their nutritional value from images.

🧠 Core Features
- Image Classification Model: A convolutional neural network (CNN) architecture featuring Conv2D, MaxPooling, and Dropout layers, trained to recognize common fruits like apples, bananas, and mangoes.

- Custom Dataset: Constructed using a combination of web scraping and real-world photos of fruits taken at home for diverse training inputs.

- Dynamic Calorie Estimation: Calculates calorie values based on the detected food class and optional weight input, ensuring flexibility in user interaction.

- UI: Angular-powered interface allows users to upload food images with a live preview before submission.

- Backend Integration: ASP.NET Core API processes the uploaded images, triggers the ML model, and returns results including prediction confidence and calorie data.
