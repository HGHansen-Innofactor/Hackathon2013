import json
import openai
import pandas as pd
from sklearn.preprocessing import MultiLabelBinarizer

N_CANDIDATES     = 25 
# Stripper, prostitue The response was filtered due to the prompt triggering Azure OpenAI’s content management policy.
#ROLE            = "HR Manager"
#REQUIRED_SKILLS = ["onboarding", "talent acquisition", "conflict resolution", "interviewing"]
ROLE             = "data scientist"
REQUIRED_SKILLS  = ["Python", "R", "SQL", "machine learning", "statistical analysis", "big data", "deep learning", "data analysis", "data cleaning"]
#ROLE            = "Clown"
#REQUIRED_SKILLS = ["face painting", "fire juggling", "magic", "mime", "pranks", "storytelling", "unicycling", "acrobatics", "balloon twisting", "jokes"]

def get_candidates(n_candidates, role):
    
    openai.api_type = "azure"
    openai.api_base = "https://openai-henriksbodega.openai.azure.com/"
    openai.api_version = "2023-03-15-preview"
    openai.api_key = "878c71d3f0a249e39c2332a8f12bad2a"

    response = openai.ChatCompletion.create(
      engine="GPT-Test1",
      messages = [{"role":"user", "content": f'''create an example JSON object called candidates 
                   with {n_candidates} children, each child has 
                   a property named "name" containing a Disney name
                   a property named "available" containing true or false and 
                   a property named "skills" containing skills relavant for a {role}'''}],
      temperature=0.7,
      max_tokens=3000,
      top_p=0.95,
      frequency_penalty=0,
      presence_penalty=0,
      stop=None)

    json_string = response.choices[0].message.content
                   
    return json.loads(json_string)
    
def one_hot_encode_dataframe(df):
    
    mlb = MultiLabelBinarizer(sparse_output=True)
    
    return df.join(
                pd.DataFrame.sparse.from_spmatrix(
                    mlb.fit_transform(df.pop('skills')),
                    index=df.index,
                    columns=mlb.classes_))

def clean_candidate_skills():
    
    candidates_required_skills              = pd.DataFrame()
    candidates_required_skills['name']      = candidates_all_skills['name']
    candidates_required_skills['available'] = candidates_all_skills['available']
    
    for required_skill in REQUIRED_SKILLS:
            if required_skill in skills_from_candidates: 
                candidates_required_skills[required_skill] = candidates_all_skills[required_skill]
            else:
                candidates_required_skills[required_skill] = 0   
    
    candidates_required_skills['points'] = candidates_required_skills[REQUIRED_SKILLS].sum(axis=1)
                
    return candidates_required_skills

candidates_json = get_candidates(N_CANDIDATES, ROLE)
candidates_all_skills = pd.json_normalize(candidates_json, record_path =['candidates'])   
candidates_all_skills = one_hot_encode_dataframe(candidates_all_skills)

skills_from_candidates = candidates_all_skills.columns.tolist()
skills_from_candidates = skills_from_candidates[2:]

candidates_required_skills = clean_candidate_skills()

candidates_required_skills = candidates_required_skills.sort_values(by='points', ascending=False)

for _i in range(5):
    print(candidates_required_skills.iloc[_i]['name'], round(candidates_required_skills.iloc[_i]['points']/float(len(REQUIRED_SKILLS))*100), "%" )
