export type FormResponse = {
    id: string;
    formId: string;
    answers: FormAnswer[];
};

export type FormAnswer = {
    sectionId: string;
    controlId: string;
    answer: string;
};
