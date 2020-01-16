import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MDBBootstrapModule, MdbInput } from 'angular-bootstrap-md';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { RegisterComponent } from './register/register.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { AdminListComponent } from './tasks/admin-list/admin-list.component';
import { AddReviewersAndEditorsComponent } from './tasks/add-reviewers-and-editors/add-reviewers-and-editors.component';
import { CorrectMagazineDataComponent } from './tasks/correct-magazine-data/correct-magazine-data.component';
import { EditorListComponent } from './tasks/editor-list/editor-list.component';
import { AddMagazineComponent } from './tasks/add-magazine/add-magazine.component';
import { TokenInterceptor } from './services/token.interceptor';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    RegisterComponent,
    LoginComponent,
    HomeComponent,
    AddMagazineComponent,
    AdminListComponent,
    EditorListComponent,
    AddReviewersAndEditorsComponent,
    CorrectMagazineDataComponent,
    AdminListComponent,
    AddReviewersAndEditorsComponent,
    CorrectMagazineDataComponent,
    EditorListComponent,
    AddMagazineComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    MDBBootstrapModule.forRoot(),
    ToastrModule.forRoot(),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
