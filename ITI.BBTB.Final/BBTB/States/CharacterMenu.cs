using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BBTB.Controls;

namespace BBTB.States
{
    public class CharacterMenu : State

    {
        private SpriteFont font;
        private List<Component> _components;
        private Texture2D backGround;
        Player _player;
        Vector2 position;
        string _classeSelected;

        public CharacterMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var archerTexture = content.Load<Texture2D>("Character/Archer");
            var mageTexture = content.Load<Texture2D>("Character/Mage");
            var gunnerTexture = content.Load<Texture2D>("Character/Gunner");
            var buttonFont = _content.Load<SpriteFont>("Font/Font");
            font = _content.Load<SpriteFont>("Font/Character");
            backGround = _content.Load<Texture2D>("Background/Character");
            position = new Vector2(0, 0);
            

            var newArcher = new Button(archerTexture, buttonFont)
            {
                Position = new Vector2(0, 75),
            };
            newArcher.Click += NewArcher_Click;

            var newMage = new Button(mageTexture, buttonFont)
            {
                Position = new Vector2(300, 75),
            };
            newMage.Click += NewMage_Click;
            
            var newGunner = new Button(gunnerTexture, buttonFont)
            {
                Position = new Vector2(600, 75),
            };
            newGunner.Click += NewGunner_Click;

            _components = new List<Component>()
            {
               
                newMage,
                newArcher,
                newGunner,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
         
            spriteBatch.Draw(backGround, position, Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, "Select your class", new Vector2(300, 10), Color.LightGoldenrodYellow);
            spriteBatch.End();
        }
        private void NewMage_Click(object sender, EventArgs e)
        {
            _classeSelected = "mage";
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _classeSelected));    
           
        }

        private void NewArcher_Click(object sender, EventArgs e)
        {
            _classeSelected = "archer";
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _classeSelected));
            
        }

        private void NewGunner_Click(object sender, EventArgs e)
        {
            _classeSelected = "gunner";
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _classeSelected));
            
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
