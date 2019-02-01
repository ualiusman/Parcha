import { Component, Inject, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { HttpClient } from "@angular/common/http";
@Component({
  selector: "quiz-searched",
  templateUrl: './quiz-searched.component.html',
  styleUrls: ['./quiz-searched.component.css']
})
export class QuizSearchedComponent implements OnInit {
    
  quizzes: Quiz[];
  http: HttpClient;
  baseUrl: string;
  searchText: string;

  constructor(private activatedRoute: ActivatedRoute,
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router) {
    this.http = http;
    this.baseUrl = baseUrl;





    //var url = this.baseUrl + "api/quiz/" + searchText + "/";
    //this.http.get<Quiz[]>(url).subscribe(result => {
    //  this.quizzes = result;
    //}, error => console.error(error));
  }

  ngOnInit(): void {
    this.searchText = this.activatedRoute.snapshot.params["text"];
    console.log(this.searchText);
  }

 
}


