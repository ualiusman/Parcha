import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import { QuizListComponent } from './components/quiz/quiz-list.component';
import { QuizComponent } from './components/quiz/quiz.component';
import { QuizEditComponent } from './components/quiz/quiz-edit.component';

import { LoginComponent } from './components/login/login.component';
import { AboutComponent } from './components/about/about.component';
import { PageNotFoundComponent } from './components/pagenotfound/pagenotfound.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    QuizListComponent,
    QuizComponent,
    QuizEditComponent,

    LoginComponent,
    AboutComponent,
    PageNotFoundComponent
    //PageNotFoundComponent,
    //CounterComponent,
    //FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'quiz/create', component: QuizEditComponent },
      { path: 'quiz/:id', component: QuizComponent },
      { path: 'quiz/edit/:id', component: QuizEditComponent },

      { path: 'login', component: LoginComponent },
      { path: 'about', component: AboutComponent },
      { path: '**', component: PageNotFoundComponent }
      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
