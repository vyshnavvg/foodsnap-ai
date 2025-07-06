export type FileInfo = {
    foodName: string;
    confidenceScore: Number;
    calories: Number;
    fullPrediction: FullPrediction
}

export type FullPrediction = {
    apple: Number;
    mango: Number;
    banana: Number;
}