using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BBTB.Controls;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BBTB.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        Texture2D backGround;
        Vector2 position;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
           
            backGround = _content.Load<Texture2D>("Background/Menu");
            position = new Vector2(0, 0);
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Font/Font");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                    Position = new Vector2(240, 100),
                    Text = "New Game",
                    PenColor = Color.BurlyWood,
            };
            newGameButton.Click += NewGameButton_Click;
          
            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(240, 250),
                Text = "Load Game",
                PenColor =  Color.BurlyWood,
            };
            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(240, 400),
                Text = "Quit",
                PenColor = Color.BurlyWood,
            };
            quitGameButton.Click += QuitGameButton_Click;



            _components = new List<Component>()
            {
               
                newGameButton,
                loadGameButton,
                quitGameButton,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backGround, position, Color.White);
            foreach (var component in _components)  
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
		{ 
			Board _board;
			PlayerModel Hero2;
			BinaryFormatter f = new BinaryFormatter();
			using (var stream = File.OpenRead("Content/Saves/Saves"))
			{
				Hero2 = (PlayerModel)f.Deserialize(stream);
				_board = (Board)f.Deserialize(stream);
			}
			Hero2.StageAndRoom();
			_board.Special = Hero2.Room;
			_board.RoomNumber--;
			_board.NewRoom();
		}

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CharacterMenu(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

  
  

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

    }
}
