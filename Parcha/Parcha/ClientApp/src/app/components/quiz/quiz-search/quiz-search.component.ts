import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
@Component({
  selector: "quiz-search",
  templateUrl: './quiz-search.component.html',
  styleUrls: ['./quiz-search.component.css']
})
export class QuizSearchComponent {
  searchText: string;
  @Input() class: string;
  @Input() placeholder: string;

  constructor(private router: Router) {

  }

  public search() {
    console.log(this.searchText);
    this.router.navigate(["quizsearched", this.searchText]);
  }
}


