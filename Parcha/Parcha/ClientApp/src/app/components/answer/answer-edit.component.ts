import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { FormGroup, FormControl, FormBuilder, Validators } from
  '@angular/forms';
@Component({
  selector: "answer-edit",
  templateUrl: './answer-edit.component.html',
  styleUrls: ['./answer-edit.component.css']
})
export class AnswerEditComponent {
  title: string;
  answer: Answer;
  form: FormGroup;
  // this will be TRUE when editing an existing answer,
  // FALSE when creating a new one.
  editMode: boolean;
  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private fb: FormBuilder,
    @Inject('BASE_URL') private baseUrl: string) {
    // create an empty object from the Question interface
    this.answer = <Answer>{};

    // initialize the form
    this.createForm();

    var id = +this.activatedRoute.snapshot.params["id"];
    // check if we're in edit mode or not
    this.editMode = (this.activatedRoute.snapshot.url[1].path ===
      "edit");
    if (this.editMode) {
      // fetch the question from the server
      var url = this.baseUrl + "api/answer/" + id;
      this.http.get<Answer>(url).subscribe(res => {
        this.answer = res;
        this.title = "Edit - " + this.answer.Text;


        // update the form with the quiz value
        this.updateForm();

      }, error => console.error(error));
    }
    else {
      this.answer.QuestionId = id;
      this.title = "Create a new Answer";
    }
  }
  onSubmit(answer: Answer) {

    // build a temporary quiz object from form values
    var tempAnswer = <Answer>{};
    tempAnswer.QuestionId = this.answer.QuestionId;
    tempAnswer.Text = this.form.value.Text;
    tempAnswer.Value = this.form.value.Value;


    var url = this.baseUrl + "api/answer";
    if (this.editMode) {
      this.http
        .post<Answer>(url, tempAnswer)
        .subscribe(res => {
          var v = res;
          console.log("Answer " + v.Id + " has been updated.");
          this.router.navigate(["question/edit", v.QuestionId]);
        }, error => console.log(error));
    }
    else {
      this.http
        .put<Answer>(url, tempAnswer)
        .subscribe(res => {
          var v = res;
          console.log("Answer " + v.Id + " has been created.");
          this.router.navigate(["question/edit", v.QuestionId]);
        }, error => console.log(error));
    }
  }
  onBack() {
    this.router.navigate(["question/edit", this.answer.QuestionId]);
  }

  createForm() {
    this.form = this.fb.group({
      Text: ['', Validators.required],
      Value: ['',
        [Validators.required,
        Validators.min(-5),
        Validators.max(5)]
      ]
    });
  }

  updateForm() {
    this.form.setValue({
      Value: this.answer.Value,
      Text: this.answer.Text || ''
    });
  }

  getFormControl(name: string) {
    return this.form.get(name);
  }

  isValid(name: string) {
    var e = this.getFormControl(name);
    return e && e.valid;
  }

  isChanged(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched);
  }

  hasError(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }
}
