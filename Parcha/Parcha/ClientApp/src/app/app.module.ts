import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth.interceptor';
import { AuthResponseInterceptor } from './services/auth.response.interceptor';

import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import { QuizListComponent } from './components/quiz/quiz-list.component';
import { QuizComponent } from './components/quiz/quiz.component';
import { QuizEditComponent } from './components/quiz/quiz-edit.component';
import { QuizSearchComponent } from './components/quiz/quiz-search.component';

import { QuestionListComponent } from './components/question/question-list.component';import { QuestionEditComponent } from './components/question/question-edit.component';
import { AnswerListComponent } from './components/answer/answer-list.component';
import { AnswerEditComponent } from './components/answer/answer-edit.component';

import { ResultListComponent } from './components/result/result-list.component';
import { ResultEditComponent } from './components/result/result-edit.component';

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
    QuizSearchComponent,

    QuestionListComponent,
    QuestionEditComponent,

    AnswerListComponent,
    AnswerEditComponent,

    ResultListComponent,
    ResultEditComponent,

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
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'quiz/create', component: QuizEditComponent },
      { path: 'quiz/:id', component: QuizComponent },
      { path: 'quiz/edit/:id', component: QuizEditComponent },


      { path: 'question/create/:id', component: QuestionEditComponent },
      { path: 'question/edit/:id', component: QuestionEditComponent },

      { path: 'answer/create/:id', component: AnswerEditComponent },
      { path: 'answer/edit/:id', component: AnswerEditComponent },

      {
        path: 'result/create/:id', component: ResultEditComponent
      },
      {
        path: 'result/edit/:id', component: ResultEditComponent
      },

      { path: 'login', component: LoginComponent },
      { path: 'about', component: AboutComponent },
      { path: '**', component: PageNotFoundComponent }
      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthResponseInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
