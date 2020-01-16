import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-reviewers-and-editors',
  templateUrl: './add-reviewers-and-editors.component.html',
  styleUrls: ['./add-reviewers-and-editors.component.scss']
})
export class AddReviewersAndEditorsComponent implements OnInit {
  addReviewersCreateForm: FormGroup;
  reviewersList = null;
  editorsList = null;
  id: number;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute, private router: Router, private toastrService: ToastrService)  { }

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.initForm();
      }
    )
  }

  initForm() {
    let reviewersArray = new FormArray([]);
    reviewersArray.push(new FormGroup({
      'reviewerName': new FormControl(null, Validators.required)
    }));

    reviewersArray.push(new FormGroup({
      'reviewerName': new FormControl(null, Validators.required)
    }));

    let editorsArray = new FormArray([]);

    this.addReviewersCreateForm = new FormGroup({
      'reviewers': reviewersArray,
      'editors' : editorsArray
    })

    this.httpClient.get(`https://localhost:44372/api/user/editors/${this.id}`).subscribe(
        (res) => 
        {
            this.editorsList = res;
        },
        (err) => 
        {
        }
    )

    this.httpClient.get(`https://localhost:44372/api/user/reviewers/${this.id}`).subscribe(
      (res) => 
      {
          this.reviewersList = res;
      },
      (err) => 
      {
      }
    )
  }

  onAddReviewer() {
    (<FormArray>this.addReviewersCreateForm.get('reviewers')).push(
      new FormGroup({
        'reviewerName': new FormControl(null, Validators.required)
      })
    );
  }

  onAddEditor() {
    (<FormArray>this.addReviewersCreateForm.get('editors')).push(
      new FormGroup({
        'editorName': new FormControl(null, Validators.required)
      })
    );
  }

  
  onSubmit() {
    this.httpClient.post(`https://localhost:44372/api/editortask/addingReviewersForMagazin/${this.id}`, this.addReviewersCreateForm.value).subscribe(
      (res) => 
      {
          this.toastrService.success('Successfully added editors and reviewers.', 'Success', { progressBar: true });
          this.router.navigate(['home']);
      },
      (err) => 
      {
        this.toastrService.error('Could not add editors and reviewers.', 'Error', { progressBar: true });
      }
  );
  }
}