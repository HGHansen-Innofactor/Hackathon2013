import openai

openai.api_type = "azure"
openai.api_base = "https://openai-henriksbodega.openai.azure.com/"
openai.api_version = "2023-03-15-preview"
openai.api_key = "878c71d3f0a249e39c2332a8f12bad2a"

response = openai.ChatCompletion.create(
  engine="GPT-Test1",
  messages = [{"role":"user","content":"give me a list of 5 beer brands"}],
  temperature=0.7,
  max_tokens=3000,
  top_p=0.95,
  frequency_penalty=0,
  presence_penalty=0,
  stop=None)

response = response.choices[0].message.content