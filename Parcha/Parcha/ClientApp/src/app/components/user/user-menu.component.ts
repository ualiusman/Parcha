import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
@Component({
  selector: "user-menu",
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.css']
})
export class UserMenuComponent implements OnInit {
  username: string;
  http: HttpClient;
  baseUrl: string;
  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    console.log("User menu executed..");
    var url = this.baseUrl + "api/user";

    this.http.get<string>(url).subscribe(result => {
      this.username = result;
    }, error => console.error(error));
  }
}
