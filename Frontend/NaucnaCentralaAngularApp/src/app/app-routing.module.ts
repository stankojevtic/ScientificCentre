import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RegisterComponent } from '././register/register.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { CorrectMagazineDataComponent } from './tasks/correct-magazine-data/correct-magazine-data.component';
import { AddReviewersAndEditorsComponent } from './tasks/add-reviewers-and-editors/add-reviewers-and-editors.component';
import { EditorListComponent } from './tasks/editor-list/editor-list.component';
import { AdminListComponent } from './tasks/admin-list/admin-list.component';
import { AddMagazineComponent } from './tasks/add-magazine/add-magazine.component';

const appRoutes: Routes = [
    { path: '', redirectTo:'home', pathMatch:'full'},
    { path: 'home', component: HomeComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent},
    { path: 'add-magazine', component: AddMagazineComponent},
    { path: 'admin-tasks', component: AdminListComponent},
    { path: 'editor-tasks', component: EditorListComponent},
    { path: 'add-reviewers-and-editors/:id', component: AddReviewersAndEditorsComponent },
    { path: 'correct-magazine-data/:id', component: CorrectMagazineDataComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule]
})
export class AppRoutingModule {

}